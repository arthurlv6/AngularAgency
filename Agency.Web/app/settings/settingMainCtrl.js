(function () {
    "use strict";
    var app = angular.module("main");
    app.controller("settingMainCtrl", [settingCtrl]);
    function settingCtrl() {
        var vm = this;
        vm.radioModel = 'Company';
        //console.log("company", vm.radioModel);
        vm.selected = function () {
            console.log("what", vm.radioModel);
        };
    }
}());
