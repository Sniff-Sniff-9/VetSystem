window.resizeObserver = {
    register: function (dotNetHelper) {
        function reportWidth() {
            dotNetHelper.invokeMethodAsync('UpdateWidth', window.innerWidth);
        }
        window.addEventListener('resize', reportWidth);
        reportWidth();
    }
};