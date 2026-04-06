window.watchAtlasTheme = {
    apply: function (themeKey) {
        if (!themeKey) {
            document.body.removeAttribute('data-theme');
            return;
        }

        document.body.setAttribute('data-theme', themeKey);
    }
};
