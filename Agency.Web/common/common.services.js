(function () {
    "use strict";
    angular
        .module("common.services",
                    ["ngResource"])
    	.constant("appSettings",
        {
            serverPath: "http://agency.api.arthurcv.com"
            //serverPath: "http://localhost:3823"
        });
}());
