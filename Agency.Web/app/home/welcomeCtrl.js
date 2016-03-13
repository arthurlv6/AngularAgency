//angular.module('main', ['ui.bootstrap', 'ngAnimate']);
angular.module('main').controller('welcomeCtrl', function ($scope) {
    $scope.myInterval = 100000;
    var slides = $scope.slides = [];
    $scope.addSlide = function (p) {
        slides.push(p);
    };
    $scope.addSlide({ image: 'images/0.jpg', text: 'Introduction' });
    $scope.addSlide({ image: 'images/1.jpg', text: 'Friends' });
    $scope.addSlide({ image: 'images/2.jpg', text: 'Working experience' });
    $scope.addSlide({ image: 'images/3.jpg', text: 'Development philosophy' });

    $scope.$watch('slides[0].active',
        function (active) {
            if (active) {
                $scope.bool0 = true;
                $scope.bool1 = false;
                $scope.bool2 = false;
                $scope.bool3 = false;
            }
        });
    $scope.$watch('slides[1].active',
        function (active) {
            if (active) {
                $scope.bool0 = false;
                $scope.bool1 = true;
                $scope.bool2 = false;
                $scope.bool3 = false;
            }
        });
    $scope.$watch('slides[2].active',
        function (active) {
            if (active) {
                $scope.bool0 = false;
                $scope.bool1 = false;
                $scope.bool2 = true;
                $scope.bool3 = false;
            }
        });
    $scope.$watch('slides[3].active',
        function (active) {
            if (active) {
                $scope.bool0 = false;
                $scope.bool1 = false;
                $scope.bool2 = false;
                $scope.bool3 = true;
            }
        });
    $scope.items = ['item1', 'item2', 'item3'];

    $scope.animationsEnabled = true;

    $scope.content = "";

});