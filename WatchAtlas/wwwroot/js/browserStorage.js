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
    },
    downloadTextFile: function (fileName, content, contentType) {
        try {
            var blob = new Blob([content], { type: contentType || "application/json;charset=utf-8" });
            var url = URL.createObjectURL(blob);
            var link = document.createElement("a");
            link.href = url;
            link.download = fileName;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            URL.revokeObjectURL(url);
        } catch {
        }
    }
};
