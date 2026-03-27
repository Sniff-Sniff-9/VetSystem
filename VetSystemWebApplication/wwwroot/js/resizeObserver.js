window.resizeObserver = {
    register: function (dotnetHelper) {
        const reportWidth = () => {
            if (dotnetHelper) {
                dotnetHelper.invokeMethodAsync('UpdateWidth', window.innerWidth).catch(err => console.log(err));
            }
        };

        window.addEventListener('resize', reportWidth);
        // сразу один раз
        reportWidth();
    }
};