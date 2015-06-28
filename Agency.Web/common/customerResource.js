(function () {
    "use strict";
    angular
        .module("common.services")
        .factory("customerResource",["$resource","appSettings","currentUser",customerResource]);

    function customerResource($resource, appSettings, currentUser) {
        return $resource(appSettings.serverPath + "/api/customers/:id", null,
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

