module.exports = {
    darkMode: ['class', '[data-theme="dark-soft"]'],
    content: [
        '../*.razor',
        '../Components/**/*.razor',
        '../Layout/**/*.razor',
        '../Pages/**/*.razor',
        '../Models/**/*.cs',
        '../Services/**/*.cs',
        '../State/**/*.cs',
        '../wwwroot/index.html'
    ],
    theme: {
        extend: {
            colors: {
                surface: 'rgb(var(--surface) / <alpha-value>)',
                'surface-strong': 'rgb(var(--surface-strong) / <alpha-value>)',
                'surface-soft': 'rgb(var(--surface-soft) / <alpha-value>)',
                'surface-soft-strong': 'rgb(var(--surface-soft-strong) / <alpha-value>)',
                text: 'rgb(var(--text-color) / <alpha-value>)',
                muted: 'rgb(var(--muted-text) / <alpha-value>)',
                accent: 'rgb(var(--accent) / <alpha-value>)',
                'accent-soft': 'rgb(var(--accent-soft) / <alpha-value>)',
                border: 'rgb(var(--border-color) / <alpha-value>)',
                success: 'rgb(var(--success) / <alpha-value>)'
            },
            fontFamily: {
                display: ['"Aptos Display"', '"Trebuchet MS"', 'sans-serif'],
                body: ['"Aptos"', '"Segoe UI Variable"', '"Segoe UI"', 'sans-serif']
            },
            boxShadow: {
                soft: '0 20px 60px rgba(15, 23, 42, 0.12)',
                float: '0 16px 40px rgba(59, 130, 246, 0.14)'
            },
            backgroundImage: {
                'hero-mist': 'radial-gradient(circle at top left, rgba(255,255,255,0.65), transparent 42%), radial-gradient(circle at right, rgba(125,211,252,0.26), transparent 28%), linear-gradient(180deg, rgba(255,255,255,0.32), rgba(255,255,255,0))'
            },
            keyframes: {
                drift: {
                    '0%, 100%': { transform: 'translate3d(0, 0, 0)' },
                    '50%': { transform: 'translate3d(0, -8px, 0)' }
                },
                rise: {
                    '0%': { opacity: '0', transform: 'translateY(10px)' },
                    '100%': { opacity: '1', transform: 'translateY(0)' }
                }
            },
            animation: {
                drift: 'drift 8s ease-in-out infinite',
                rise: 'rise 0.45s ease-out forwards'
            }
        },
    },
    plugins: [],
}
