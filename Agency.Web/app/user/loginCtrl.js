(function () {
    "use strict";
    var app = angular.module("main");
    app.controller('ModalRegisterCtrl', function ($scope, $modal, currentUser, userAccount) {
        var html = "";
        $scope.isLoggedIn = function () {
            return currentUser.getProfile().isLoggedIn;
        };
        $scope.open = function (type) {
            if (type === "reg") {
                html = "/app/user/ModalRegisterContent.html";
            } else {
                html = "/app/user/ModalLoginContent.html";
            }
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: html,
                controller: 'ModalRegisterInstanceCtrl',
                size: ''
            });
        };
        $scope.logout = function () {
            var user = currentUser.getProfile();
            user.isLoggedIn = false;
            user.userName = '';
            user.access_token = '';
        };
    });
    app.controller('ModalRegisterInstanceCtrl', ["$scope", "$modalInstance", "userAccount", "currentUser", ModalRegisterInstanceCtrl]);
    function ModalRegisterInstanceCtrl($scope, $modalInstance,userAccount, currentUser) {
        
        $scope.message = '';
        $scope.userData = {
            email: '',
            password: '',
            confirmPassword: ''
        };

        $scope.registerUser = function () {
            userAccount.registration.registerUser($scope.userData,
                function (data) {
                    $scope.confirmPassword = "";
                    $scope.message = "... Registration successful";
                    $scope.login();
                    
                },
                function (response) {
                    $scope.isLoggedIn = false;
                    $scope.message = response.statusText + "\r\n";
                    if (response.data.exceptionMessage)
                        $scope.message += response.data.exceptionMessage;

                    // Validation errors
                    if (response.data.modelState) {
                        for (var key in response.data.modelState) {
                            $scope.message += response.data.modelState[key] + "\r\n";
                        }
                    }
                });
        };
        $scope.login = function () {
            $scope.userData.grant_type = "password";
            $scope.userData.userName = $scope.userData.email;
            userAccount.login.loginUser($scope.userData,
                function (data) {
                    $scope.password = "";
                    currentUser.setProfile($scope.userData.userName, data.access_token);
                    $scope.message = "";
                    $modalInstance.dismiss('ok');
                },
                function (response) {
                    $scope.password = "";
                    $scope.message = response.statusText + "\r\n";
                    if (response.data.exceptionMessage)
                        $scope.message += response.data.exceptionMessage;

                    if (response.data.error) {
                        $scope.message += response.data.error;
                    }
                });
        };
        
        $scope.cancel = function () {
            console.log($scope.userData.email);
            console.log("dismiss");
            $modalInstance.dismiss('cancel');
        };
    }
})();