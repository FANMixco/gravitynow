function getLanguage()
{
	var validLanguages=new Array("","es", "en");	
	
	var language = window.navigator.userLanguage || window.navigator.language;
	language=language.substring(0,2);

	if (validLanguages.indexOf(language)==0)
		language="en";

	return "res/scripts/maps/resources/resource."+language+".js";

}

function getCurrent_Language()
{
	var validLanguages=new Array("","es", "en");	
	
	var language = window.navigator.userLanguage || window.navigator.language;
	language=language.substring(0,2);

	if (validLanguages.indexOf(language)==0)
		language="en";

	return "res/scripts/maps/resources/resource."+language+".js";

}