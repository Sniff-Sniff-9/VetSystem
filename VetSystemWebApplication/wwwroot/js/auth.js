window.auth = {
    getToken: function () {
        const match = document.cookie.match(/jwtToken=([^;]+)/);
        return match ? match[1] : "";
    }
};