<?php
function query2JSON($sql,$callback='') {
	$rows = $this->fetch_all_array($sql);
	$total_rows=count($rows);
	$i=0;
	//$str=$callback."({\r\n\tstatus: \"".(($this->query_id != 0) ? 1 : 0)."\",\r\n\titems: [";
	foreach($rows as $k){
		$j=0;
		$total_colums = count($k);
		$str .= "\r\n\t\t{ ";
		foreach($k as $key => $val) {
			$str .= $key." : \"".$val."\"";
			$j++;
			if($j<$total_colums) $str .= ",";
		}
		$str .= " }";
		$i++;
		if($i<$total_rows) $str .= ",";
	}
	//$str .="\r\n\t]\r\n})";
	return $str;
}
?>