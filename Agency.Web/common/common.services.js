(function () {
    "use strict";
    angular
        .module("common.services",
                    ["ngResource"])
    	.constant("appSettings",
        {
            //serverPath: "http://webapi.arthurcv.com"
            serverPath: "http://localhost:3823"
        });
}());
