window.GoogleAnalyticsObject = "__ga__";
window.__ga__ = function() {
  for (var i=0; i<arguments.length; i++) {
    var arg = arguments[i];
    if (arg.constructor == Object && arg.hitCallback) {
      arg.hitCallback();
    }
  }
};
window.__ga__.q = [["create", "UA-3532277-2", "auto"]];
window.__ga__.l = Date.now();

requirejs.config({
  baseUrl: "js",
  paths: {
    jquery: "https://code.jquery.com/jquery-3.1.0.min",
    modernizr: "lib/modernizr.min",
    highlight: "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.4.0/highlight.min",
    "ga": [
      "https://www.google-analytics.com/analytics",
      "data:application/javascript,"
    ],
    jscookie: "lib/js.cookie-2.1.3.min",
  },
  shim: {
    "ga": {
      exports: "__ga__"
    }
  },
});

requirejs(["app"], function(app) {
  app.start();
});