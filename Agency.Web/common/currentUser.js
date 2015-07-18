(function () {
    "use strict";

    angular
        .module("common.services")
        .factory("currentUser",
            currentUser);

    function currentUser($cookieStore) {
        var profile = {
            isLoggedIn: false,
            username: "",
            token: ""
        };

        var setProfile = function (username, token) {
            profile.username = username;
            profile.token = token;
            profile.isLoggedIn = true;
            $cookieStore.put("currentUser", profile);
        };

        var getProfile = function () {
            var currentUser=$cookieStore.get("currentUser");
            if (currentUser) {
                return currentUser;
            } else {
                return {
                    isLoggedIn: false,
                    username: "",
                    token: ""};
            }
            //return $cookieStore.get("currentUser");
        };
        var removeProfile = function () {
            return $cookieStore.remove("currentUser");
        };
        return {
            setProfile: setProfile,
            getProfile: getProfile,
            removeProfile: removeProfile
        };
    }
})();
