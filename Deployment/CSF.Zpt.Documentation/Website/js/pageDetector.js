define(["jquery", "config/pageScriptRegistry"], function($, registry) {
  "use strict";
  
  function getPageName()
  {
    var pageNameMatcher = /page_([a-zA-Z0-9_-]+)/;
    
    var bodyElement = $("body");
    var classes = bodyElement.attr("class").split(" ");
    
    for(var i = 0, classLength = classes.length; i < classLength; i++)
    {
      var currentClass = classes[i];
      var currentMatch = currentClass.match(pageNameMatcher);
      if(currentMatch)
      {
        return currentMatch[1];
      }
    }
    
    return null;
  }
  
  function initialisePage()
  {
    loadPageModule(function(pageModule) {
      if(pageModule && pageModule.initialise)
      {
        pageModule.initialise();
      }
    });
  }
  
  function loadPageModule(callback)
  {
    var pageName = getPageName();
    if(registry.indexOf(pageName) < 0)
    {
      return;
    }
    
    var pageModulePath = "pages/" + pageName;
    require([pageModulePath], callback);
  }
  
  return {
    getPageName: getPageName,
    initialiseCurrentPage: initialisePage,
  };
});