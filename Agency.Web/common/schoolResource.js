(function () {
    "use strict";
    angular
        .module("common.services")
        .factory("schoolResource",
            ["$resource",
                "appSettings",
                "currentUser",
                schoolResource]);

    function schoolResource($resource, appSettings, currentUser) {
        return $resource(appSettings.serverPath + "/api/schools/:id", null,
            {
                'get': {
                    headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().token }
                },

                'save': {
                    method: 'POST',
                    headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().token }
                },

                'update': {
                    method: 'PUT',
                    headers: { 'Authorization': 'Bearer ' + currentUser.getProfile().token }
                }
            });
    }
}());

