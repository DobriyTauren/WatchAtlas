window.watchAtlasTheme = {
    settingsKey: 'watchatlas.settings',
    defaultThemeKey: 'light-soft',
    transitionBlockerId: 'watchatlas-theme-transition-blocker',
    apply: function (themeKey, accentHex) {
        var resolvedTheme = themeKey || this.defaultThemeKey;
        var releaseTransitions = this.freezeTransitions();

        document.documentElement.setAttribute('data-theme', resolvedTheme);
        if (document.body) {
            document.body.setAttribute('data-theme', resolvedTheme);
        }

        if (accentHex) {
            var themeColorMeta = document.querySelector('meta[name="theme-color"]');
            if (themeColorMeta) {
                themeColorMeta.setAttribute('content', accentHex);
            }
        }

        releaseTransitions();
    },
    freezeTransitions: function () {
        var existing = document.getElementById(this.transitionBlockerId);
        if (existing) {
            existing.remove();
        }

        var style = document.createElement('style');
        style.id = this.transitionBlockerId;
        style.textContent = '*,*::before,*::after{transition:none!important;animation:none!important;}';
        document.head.appendChild(style);

        if (document.body) {
            void document.body.offsetHeight;
        } else {
            void document.documentElement.offsetHeight;
        }

        return function () {
            requestAnimationFrame(function () {
                requestAnimationFrame(function () {
                    style.remove();
                });
            });
        };
    },
    bootstrap: function () {
        try {
            var rawSettings = window.localStorage.getItem(this.settingsKey);
            if (!rawSettings) {
                this.apply(this.defaultThemeKey, '#d17187');
                return;
            }

            var settings = JSON.parse(rawSettings);
            switch (settings && settings.themeMode) {
                case 1:
                    this.apply('dark-soft', '#84abff');
                    return;
                case 2:
                    this.apply('pastel', '#997de0');
                    return;
                default:
                    this.apply('light-soft', '#d17187');
                    return;
            }
        } catch {
            this.apply(this.defaultThemeKey, '#d17187');
        }
    }
};
