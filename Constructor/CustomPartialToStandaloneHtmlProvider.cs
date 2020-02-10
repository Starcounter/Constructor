﻿using System;
using Starcounter;

namespace Constructor
{
    public class CustomPartialToStandaloneHtmlProvider : PartialToStandaloneHtmlProvider
    {
        const string ImplicitStandaloneTemplate = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
    <title>{0}</title>
    <script src=""/sys/webcomponentsjs/webcomponents-lite.js""></script>
    <script>
      /* this script must run before Polymer is imported */
      /*
       * Let Polymer use native Shadow DOM if available.
       * Otherwise (at least Polymer 1.x) assumes everybody else
       * uses ShadyDOM, which is not true, as many Vanilla CE uses
       * real Shadow DOM.
       */
      window.Polymer = {{
        dom: ""shadow""
      }};
    </script>
    <link rel=""import"" href=""/sys/polymer/polymer.html"">
    <link rel=""import"" href=""/sys/starcounter.html"">
    <style>
        body {{
            margin: 20px;
        }}
        body > starcounter-include{{
            height: 100%;
        }}
    </style>
</head>
<body>
    <dom-bind id=""palindrom-root"">
        <template is=""dom-bind"">
            <starcounter-include view-model=""{{{{model}}}}""></starcounter-include>
        </template>
    </dom-bind>
    <palindrom-client ref=""palindrom-root"" remote-url=""{1}""></palindrom-client>
</body>
</html>";

        public CustomPartialToStandaloneHtmlProvider()
            : base(ImplicitStandaloneTemplate)
        {
        }
    }
}
