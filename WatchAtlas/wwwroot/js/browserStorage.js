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
    getPreferredLanguage: function () {
        try {
            var language = (navigator.languages && navigator.languages.length > 0
                ? navigator.languages[0]
                : navigator.language) || "en";

            return language.toLowerCase();
        } catch {
            return "en";
        }
    },
    setDocumentLanguage: function (language) {
        try {
            if (document && document.documentElement) {
                document.documentElement.lang = language || "en";
            }
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
    },
    copyText: async function (value) {
        try {
            if (!value) {
                return;
            }

            if (navigator.clipboard && typeof navigator.clipboard.writeText === "function") {
                await navigator.clipboard.writeText(value);
                return;
            }

            var textarea = document.createElement("textarea");
            textarea.value = value;
            textarea.setAttribute("readonly", "");
            textarea.style.position = "absolute";
            textarea.style.left = "-9999px";
            document.body.appendChild(textarea);
            textarea.select();
            document.execCommand("copy");
            document.body.removeChild(textarea);
        } catch {
        }
    },
    focusModal: function (element) {
        try {
            if (!element) {
                return;
            }

            element.scrollIntoView({
                behavior: "smooth",
                block: "center",
                inline: "nearest"
            });

            if (typeof element.focus === "function") {
                window.setTimeout(function () {
                    element.focus({ preventScroll: true });
                }, 120);
            }
        } catch {
        }
    }
};
