﻿(function () {
    "use strict";

    angular
        .module("common.services")
        .factory("userAccount",
            ["$resource",
                "appSettings",
                "currentUser",
                userAccount]);

    function userAccount($resource, appSettings,currentUser) {
        return {
            registration: $resource(appSettings.serverPath + "/api/Account/Register", null,
                {
                    'registerUser': { method: 'POST' }
                }),
            login: $resource(appSettings.serverPath + "/Token", null,
                {
                    'loginUser': {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        transformRequest: function(data, headersGetter) {
                            var str = [];
                            for (var d in data)
                                str.push(encodeURIComponent(d) + "=" +
                                    encodeURIComponent(data[d]));
                            return str.join("&");
                        }
                    }
                })
            //,
            //logout: $resource(appSettings.serverPath + "/api/Account/Logout", null,
            //    {
            //        'logoutUser': {
            //            method: 'POST',
            //            headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().token }
            //        }
            //    })
        };
    }
})();
