window.watchAtlasStorage = {
    getItem: function (key) {
        try {
            return window.localStorage.getItem(key);
        } catch {
            return null;
        }
    },
    setItem: function (key, value) {
        try {
            window.localStorage.setItem(key, value);
        } catch {
        }
    },
    removeItem: function (key) {
        try {
            window.localStorage.removeItem(key);
        } catch {
        }
    }
};
