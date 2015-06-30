(function () {
    "use strict";
    angular
        .module("common.services")
        .factory("schoolTypesResource", ["$resource", "appSettings", "currentUser", schoolTypesResource]);

    function schoolTypesResource($resource, appSettings, currentUser) {
        return $resource(appSettings.serverPath + "/api/schoolTypes/:id", null,
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

