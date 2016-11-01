define(["jquery", "config/pageScriptRegistry"], function($, registry) {
  "use strict";
  
  function getPageName()
  {
    var
      pageNameMatcher = /page_([a-zA-Z0-9_-]+)/,
      bodyElement = $("body"),
      bodyClass = bodyElement.attr("class"),
      classes = (bodyClass || "").split(" ");
    
    for(var i = 0, classLength = classes.length; i < classLength; i++)
    {
      var
        currentClass = classes[i],
        currentMatch = currentClass.match(pageNameMatcher);
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
    var
      pageName = getPageName(),
      pageModulePath = "pages/" + pageName;
    
    if(registry.indexOf(pageName) < 0)
    {
      return;
    }
    
    
    require([pageModulePath], callback);
  }
  
  return {
    getPageName: getPageName,
    initialiseCurrentPage: initialisePage,
  };
});