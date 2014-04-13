/**
 * @name MarkerClusterer
 * @version 1.0
 * @author Xiaoxi Wu
 * @copyright (c) 2009 Xiaoxi Wu
 * @fileoverview
 * This javascript library creates and manages per-zoom-level 
 * clusters for large amounts of markers (hundreds or thousands).
 * This library was inspired by the <a href="http://www.maptimize.com">
 * Maptimize</a> hosted clustering solution.
 * <br /><br/>
 * <b>How it works</b>:<br/>
 * The <code>MarkerClusterer</code> will group markers into clusters according to
 * their distance from a cluster's center. When a marker is added,
 * the marker cluster will find a position in all the clusters, and 
 * if it fails to find one, it will create a new cluster with the marker.
 * The number of markers in a cluster will be displayed
 * on the cluster marker. When the map viewport changes,
 * <code>MarkerClusterer</code> will destroy the clusters in the viewport 
 * and regroup them into new clusters.
 *
 */

/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * DERIVATION OF THE ABOVE
 * =======================
 * The standard library noted above has been  modified to work with the Nokia Maps API
 *
 */




/**
 * Creates a new MarkerClusterer to cluster markers on the map.
 *
 * @constructor
 * @param {nokia.maps.map.Display } map The map that the markers should be added to.
 * @param {Array of nokia.maps.map.Marker } opt_markers Initial set of markers to be clustered.
 * @param {MarkerClustererOptions} opt_opts A container for optional arguments.
 */
function MarkerClusterer(map, opt_markers) {

  ExtendedOList.prototype = new nokia.maps.util.OList();
  function ExtendedOList (){
  		this.addAll = function (elements){
  			  ExtendedOList.prototype.addAll.call(this, elements);
  				 addMarkers(elements);
  		}
  		this.add = function (element){
  			  ExtendedOList.prototype.add.call(this, element);
  				 addMarker(element);
  		}
  		this.clear= function (){
				clearMarkers();
				 ExtendedOList.prototype.clear.call(this);
  	}
  	this.remove= function (element){
  			   removeMarker(element)
  				 ExtendedOList.prototype.remove.call(this);
  	}
  	this.removeAt= function (idx){
  			   removeMarker( ExtendedOList.prototype.get.call(this,idx));  			
  			   ExtendedOList.prototype.removeAt.call(this,idx);	
  	}
  	
  	this.removeAll= function (elements){
  			for (var i = 0; i < elements.length; ++i) {
  			   removeMarker( elements[i]); 
  			} 			
  			ExtendedOList.prototype.removeAll.call(this,elements);	
  	}
	}
	this.objects = new ExtendedOList();  
	
	
  // private members
  var clusters_ = [];
  var map_ = map;
  var maxZoom_ = null;
  var me_ = this;
  var gridSize_ = 60;
  var sizes = [53, 56, 66, 78, 90];

  var leftMarkers_ = [];
  

  /**
   * When we add a marker, the marker may not in the viewport of map, then we don't deal with it, instead
   * we add the marker into a array called leftMarkers_. When we reset MarkerClusterer we should add the
   * leftMarkers_ into MarkerClusterer.
   */
  function addLeftMarkers_() {
    if (leftMarkers_.length === 0) {
      return;
    }
    var leftMarkers = [];
    for (i = 0; i < leftMarkers_.length; ++i) {
      addMarker(leftMarkers_[i], true, null, null, true);
    }
    leftMarkers_ = leftMarkers;
  }



  /**
   * Remove all markers from MarkerClusterer.
   */
  var clearMarkers = function () {
    for (var i = 0; i < clusters_.length; ++i) {
      if (typeof clusters_[i] !== "undefined" && clusters_[i] !== null) {
        clusters_[i].clearMarkers();
      }
    }
    clusters_ = [];
    leftMarkers_ = [];
  };

  /**
   * Check a marker, whether it is in current map viewport.
   * @private
   * @return {Boolean} if it is in current map viewport
   */
  function isMarkerInViewport_(marker) {
    return typeof map_ !== "undefined" && map_.getViewBounds().contains(marker.getBoundingBox());
  }

  /**
   * When reset MarkerClusterer, there will be some markers get out of its cluster.
   * These markers should be add to new clusters.
   * @param {Array of nokia.maps.map.Marker } markers Markers to add.
   */
  function reAddMarkers_(markers) {
    var len = markers.length;
    var clusters = [];
    for (var i = len - 1; i >= 0; --i) {
      addMarker(markers[i].marker, true, markers[i].isAdded, clusters, true);
    }
    addLeftMarkers_();
  }

  /**
   * Add a marker.
   * @private
   * @param {nokia.maps.map.Marker } marker Marker you want to add
   * @param {Boolean} opt_isNodraw Whether redraw the cluster contained the marker
   * @param {Boolean} opt_isAdded Whether the marker is added to map. Never use it.
   * @param {Array of Cluster} opt_clusters Provide a list of clusters, the marker
   *     cluster will only check these cluster where the marker should join.
   */
  var addMarker = function (marker, opt_isNodraw, opt_isAdded, opt_clusters, opt_isNoCheck) {
  	if (opt_isNoCheck !== true ) {
      if (!isMarkerInViewport_(marker)) {
        leftMarkers_.push(marker);
        return;
      }
    }

    var isAdded = opt_isAdded;
    var clusters = opt_clusters;
    var pos = map_.geoToPixel(marker.coordinate);

    if (typeof isAdded !== "boolean") {
      isAdded = false;
    }
    if (typeof clusters !== "object" || clusters === null) {
      clusters = clusters_;
    }

    var length = clusters.length;
    var cluster = null;
    for (var i = length - 1; i >= 0; i--) {
      cluster = clusters[i];
      var center = cluster.getCenter();
      if (center === null) {
        continue;
      }
      center = map_.geoToPixel(center);

      // Found a cluster which contains the marker.
      if (pos.x >= center.x - gridSize_ && pos.x <= center.x + gridSize_ &&
          pos.y >= center.y - gridSize_ && pos.y <= center.y + gridSize_) {
        cluster.addMarker({
          'isAdded': isAdded,
          'marker': marker
        });
        if (!opt_isNodraw) {
          cluster.redraw_();
        }
        return;
      }
    }

    // No cluster contain the marker, create a new cluster.
    cluster = new Cluster(me_, map_);
    cluster.addMarker({
      'isAdded': isAdded,
      'marker': marker
    });
    if (!opt_isNodraw) {
      cluster.redraw_();
    }

    // Add this cluster both in clusters provided and clusters_
    clusters.push(cluster);
    if (clusters !== clusters_) {
      clusters_.push(cluster);
    }
  };

  /**
   * Remove a marker.
   *
   * @param {nokia.maps.map.Marker } marker The marker you want to remove.
   */

  var removeMarker = function (marker) {
    for (var i = 0; i < clusters_.length; ++i) {
      if (clusters_[i].removeMarker(marker)) {
        clusters_[i].redraw_();
        return;
      }
    }
  };

  /**
   * Redraw all clusters in viewport.
   */
  var redraw_ = function () {
    var clusters = getClustersInViewport_();
    for (var i = 0; i < clusters.length; ++i) {
      clusters[i].redraw_(true);
    }
  };

  /**
   * Get all clusters in viewport.
   * @return {Array of Cluster}
   */
 var getClustersInViewport_ = function () {
    var clusters = [];
    if (map_ !== undefined){
    
		    var curBounds = map_.getViewBounds();
		
		    for (var i = 0; i < clusters_.length; i ++) {
		      if (clusters_[i].isInBounds(curBounds)) {
		        clusters.push(clusters_[i]);
		      }
		    }
  	}
    return clusters;
  };

  /**
   * Get max zoom level.
   * @private
   * @return {Number}
   */
  this.getMaxZoom_ = function () {
    return maxZoom_;
  };



  /**
   * Get grid size
   * @private
   * @return {Number}
   */
  this.getGridSize_ = function () {
    return gridSize_;
  };

  /**
   * Get total number of markers.
   * @return {Number}
   *
  this.getTotalMarkers = function () {
    var result = 0;
    for (var i = 0; i < clusters_.length; ++i) {
      result += clusters_[i].getTotalMarkers();
    }
    return result;
  };*/

  /**
   * Get total number of clusters.
   * @return {int}
   */
  this.getTotalClusters = function () {
    return clusters_.length;
  };

  /**
   * Collect all markers of clusters in viewport and regroup them.
   */
  var resetViewport = function () {
  	

    var clusters = getClustersInViewport_();
    var tmpMarkers = [];
    var removed = 0;


    for (var i = 0; i < clusters.length; ++i) {
      var cluster = clusters[i];
      var oldZoom = cluster.getCurrentZoom();
      if (oldZoom === null) {
        continue;
      }
      var curZoom = map_.zoomLevel;
      if (curZoom !== oldZoom) {

        // If the cluster zoom level changed then destroy the cluster
        // and collect its markers.
        var mks = cluster.getMarkers();
        for (var j = 0; j < mks.length; ++j) {
          var newMarker = {
            'isAdded': false,
            'marker': mks[j].marker
          };
          tmpMarkers.push(newMarker);
        }
        cluster.clearMarkers();
        removed++;
        for (j = 0; j < clusters_.length; ++j) {
          if (cluster === clusters_[j]) {
            clusters_.splice(j, 1);
          }
        }
      }
    }

    // Add the markers collected into marker cluster to reset
    reAddMarkers_(tmpMarkers);
    
    redraw_();
    

  };


  /**
   * Add a set of markers.
   *
   * @param {Array of nokia.maps.map.Marker } markers The markers you want to add.
   */
 var addMarkers = function (markers) {
    for (var i = 0; i < markers.length; ++i) {
      addMarker(markers[i], true);
    }
    redraw_();
  };

  // initialize
  if (typeof opt_markers === "object" && opt_markers !== null) {
    this.addMarkers(opt_markers);
  }

  // when map move end, regroup.
  	var eventHandler = function (evt) {     
	       		resetViewport();
				};


  
this.attach = function (mapDisplay) {
  map_ = mapDisplay;
  map_.addListener("mapviewchangeend", eventHandler); 
  map_.addObserver("zoomLevel", eventHandler);
  resetViewport();

};

this.destroy = function(){
	map_.removeListener("mapviewchangeend");
  map_.removeObserver("zoomLevel");
};

this.getId = function () {
	return 'MarkerClusterer';
};
this.getVersion = function(){
		return '1.0.0';
}; 
  

}

/**
 * Create a cluster to collect markers.
 * A cluster includes some markers which are in a block of area.
 * If there are more than one markers in cluster, the cluster
 * will create a {@link ClusterMarker_} and show the total number
 * of markers in cluster.
 *
 * @constructor
 * @private
 * @param {MarkerClusterer} markerClusterer The marker cluster object
 */
function Cluster(markerClusterer, map) {
  var center_ = null;
  var markers_ = [];
  var markerClusterer_ = markerClusterer;
  var map_ = map;
  var clusterMarker_ = null;
  var zoom_ = map_.zoomLevel;

  /**
   * Get markers of this cluster.
   *
   * @return {Array of nokia.maps.map.Marker }
   */
  this.getMarkers = function () {
    return markers_;
  };

  /**
   * If this cluster intersects certain bounds.
   *
   * @param {nokia.maps.geo.BoundingBox} bounds A bounds to test
   * @return {Boolean} Is this cluster intersects the bounds
   */
  this.isInBounds = function (bounds) {
  
    if (center_ === null) {
      return false;
    }

    if (!bounds) {
      bounds = map_.getViewBounds();
    }
    var se = map_.geoToPixel(bounds.bottomRight);
    var nw = map_.geoToPixel(bounds.topLeft);

    var centerxy = map_.geoToPixel(center_);
    var inViewport = true;
    var gridSize = markerClusterer.getGridSize_();
    if (zoom_ !== map_.zoomLevel) {
      var dl = map_.zoomLevel - zoom_;
      gridSize = Math.pow(2, dl) * gridSize;
    }

    if (se.x !== nw.x && (centerxy.x + gridSize < nw.x || centerxy.x - gridSize > se.x)) {
      inViewport = false;
    }
    if (inViewport && (centerxy.y + gridSize < nw.y || centerxy.y - gridSize > se.y)) {
      inViewport = false;
    }
    return inViewport;
  };

  /**
   * Get cluster center.
   *
   * @return {nokia.maps.geo.Coordinate}
   */
  this.getCenter = function () {
    return center_;
  };

  /**
   * Add a marker.
   *
   * @param {Object} marker An object of marker you want to add:
   *   {Boolean} isAdded If the marker is added on map.
   *   {nokia.maps.map.Marker } marker The marker you want to add.
   */
  this.addMarker = function (marker) {
    if (center_ === null) {
      center_ = marker.marker.coordinate;
    }
    markers_.push(marker);
  };

  /**
   * Remove a marker from cluster.
   *
   * @param {nokia.maps.map.Marker } marker The marker you want to remove.
   * @return {Boolean} Whether find the marker to be removed.
   */
  this.removeMarker = function (marker) {
    for (var i = 0; i < markers_.length; ++i) {
      if (marker === markers_[i].marker) {
        if (markers_[i].isAdded) {
          map_.objects.remove(markers_[i].marker);
        }
        markers_.splice(i, 1);
        return true;
      }
    }
    return false;
  };

  /**
   * Get current zoom level of this cluster.
   * Note: the cluster zoom level and map zoom level not always the same.
   *
   * @return {Number}
   */
  this.getCurrentZoom = function () {
    return zoom_;
  };

  /**
   * Redraw a cluster.
   * @private
   * @param {Boolean} isForce If redraw by force, no matter if the cluster is
   *     in viewport.
   */
  this.redraw_ = function (isForce) {
  	

    if (!isForce && !this.isInBounds()) {
      return;
    }
    // Set cluster zoom level.
    zoom_ = map_.zoomLevel;
    var i = 0;
    var mz = markerClusterer.getMaxZoom_();
    if (mz === null) {
      mz = map_.maxZoomLevel;
    }
    

    if (zoom_ >= mz || markers_.length === 1) {

      // If current zoom level is beyond the max zoom level or the cluster
      // have only one marker, the marker(s) in cluster will be showed on map.
      for (i = 0; i < markers_.length; ++i) {
        if (markers_[i].isAdded) {
          if (!markers_[i].marker.isVisible()) {
            markers_[i].marker.set("visible", true);
          }
        } else {
          map_.objects.add(markers_[i].marker);
          markers_[i].isAdded = true;
        }
      }
      if (clusterMarker_ !== null) {
        clusterMarker_.set("visible", false);
      }
    } else {
      // Else add a cluster marker on map to show the number of markers in
      // this cluster.
      for (i = 0; i < markers_.length; ++i) {
        if (markers_[i].isAdded && (markers_[i].marker.isVisible())) {
          markers_[i].marker.set("visible", false);
        }
      }
      if (clusterMarker_ === null) {
      
      	if (markers_.length > 1){
      	var markerIcon = createIcon(markers_.length);
				clusterMarker_ = new nokia.maps.map.Marker(center_ , 
							{icon: markerIcon}
				);
				
      	
      	/*
      	clusterMarker_ =
       new nokia.maps.map.StandardMarker(center_ , 
								{  
								text:  markers_.length,  
								brush: {color: '#FF0000'}            
								}); */
				 map_.objects.add (  clusterMarker_);
					}
      } else if (markers_.length < 2){
      		 map_.objects.remove (  clusterMarker_);
      	   clusterMarker_ = null;      	    
       } else {
       	  var markerIcon= createIcon(markers_.length);
      		clusterMarker_.set("icon" ,markerIcon);
        if (!clusterMarker_.isVisible()) {
          clusterMarker_.set("visible", (markers_.length > 1));
        }
      }
    }
  };

  /**
   * Remove all the markers from this cluster.
   */
  this.clearMarkers = function () {
    if (clusterMarker_ !== null) {
      map_.objects.remove(clusterMarker_);
    }
    for (var i = 0; i < markers_.length; ++i) {
      if (markers_[i].isAdded) {
        map_.objects.remove(markers_[i].marker);
      }
    }
    markers_ = [];
  };

  /**
   * Get number of markers.
   * @return {Number}
   */
  this.getTotalMarkers = function () {
    return markers_.length;
  };
  
  
  var iconSVG = 
	[
	'<svg width="51" height="51" xmlns="http://www.w3.org/2000/svg">' +
	'<circle stroke="#FFF" fill="__MAINCOLOR__" cx="25" cy="25" r="25" stroke-width="2" />' +
	'<circle stroke="#FFF" fill="__MAINCOLOR__" cx="25" cy="25" r="21" stroke-width="2"/>' +
	'<circle stroke="#FFF" fill="__MAINCOLOR__" cx="25" cy="25" r="17" stroke-width="2"/>' +
	'<text x="25" y="29" font-size="10pt" font-family="arial" font-weight="bold" text-anchor="middle" fill="#FFF" textContent="__TEXTCONTENT__">__TEXT__</text>' +
	'</svg>',
	'<svg width="47" height="47" xmlns="http://www.w3.org/2000/svg">' +
	'<circle stroke="#FFF" fill="__MAINCOLOR__" cx="23" cy="23" r="23" stroke-width="2" />' +
	'<circle stroke="#FFF" fill="__MAINCOLOR__" cx="23" cy="23" r="19" stroke-width="2"/>' +
	'<circle stroke="#FFF" fill="__MAINCOLOR__" cx="23" cy="23" r="15" stroke-width="2"/>' +
	'<text x="23" y="27" font-size="10pt" font-family="arial" font-weight="bold" text-anchor="middle" fill="#FFF" textContent="__TEXTCONTENT__">__TEXT__</text>' +
	'</svg>',
	'<svg width="43" height="43" xmlns="http://www.w3.org/2000/svg">' +
	'<circle stroke="#FFF" fill="__MAINCOLOR__" cx="21" cy="21" r="21" stroke-width="2" />' +
	'<circle stroke="#FFF" fill="__MAINCOLOR__" cx="21" cy="21" r="17" stroke-width="2"/>' +
	'<circle stroke="#FFF" fill="__MAINCOLOR__" cx="21" cy="21" r="13" stroke-width="2"/>' +
	'<text x="21" y="25" font-size="10pt" font-family="arial" font-weight="bold" text-anchor="middle" fill="#FFF" textContent="__TEXTCONTENT__">__TEXT__</text>' +
	'</svg>',
	'<svg width="39" height="39" xmlns="http://www.w3.org/2000/svg">' +
	'<circle stroke="#FFF" fill="__MAINCOLOR__" cx="19" cy="19" r="19" stroke-width="2" />' +
	'<circle stroke="#FFF" fill="__MAINCOLOR__" cx="19" cy="19" r="15" stroke-width="2"/>' +
	'<circle stroke="#FFF" fill="__MAINCOLOR__" cx="19" cy="19" r="11" stroke-width="2"/>' +
	'<text x="19" y="23" font-size="10pt" font-family="arial" font-weight="bold" text-anchor="middle" fill="#FFF" textContent="__TEXTCONTENT__">__TEXT__</text>' +
	'</svg>']
	;
	
	var stops = {
				"0": "#8A8A8A",       //Grey for 0-10
				"10": "#CACA00",  //Yellow for 11-50
				"50": "#00CC00",	//Green for 51-100
				"100": "#0000CC",  //Blue for 101-200
				"200": "#CC00CC",	//Purple for 201-500
				"500": "#FF0000"	  //Red for over 500
			};
	
	
	
var	svgParser = new nokia.maps.gfx.SvgParser(),
	// Helper function that allows us to easily set the text and color of our SVG marker.
	createIcon = function (count, mainColor) {
		
		var digit = 0;
		
		
		if (count < 10){
				digit = 3
	  } else if (count < 100){
				digit = 2
	  } else if (count < 1000){
				digit = 1;
		}
	  for (var i in stops) {
				if (count > i){
					 color = stops[i];
			  }
			}
		
		
		var svg = iconSVG[digit]
			.replace(/__TEXTCONTENT__/g, count)
			.replace(/__TEXT__/g, count)
			.replace(/__MAINCOLOR__/g, color);
		return new nokia.maps.gfx.GraphicsImage(svgParser.parseSvg(svg));
	};
  
  
}

MarkerClusterer.prototype = new nokia.maps.map.component.Component();