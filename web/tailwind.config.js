module.exports = {
    content: ["./src/**/*.{js,jsx,ts,tsx}"],
    theme: {
        extend: {
            transitionProperty: {
                'opacity': 'opacity',

                'bg': 'background-color',
            },
            transitionDuration: {
                '300': '300ms',
            },
            colors: {
                fblue: {
                    100: "#1D90F5",
                },
            },
            rotate: {
                '270': '270deg',
            },
            spacing: {
                '2.5': '0.625rem', // 10px
                '3.75': '0.9375rem', // 15px
                '1.25': '0.3125rem', // 5px
                '15': '3.75rem', // 60px
                '1.75': '0.4375rem', // 7px
            },
            keyframes: {
                fadeIn: {
                    '0%': { opacity: 0 },
                    '100%': { opacity: 1 },
                },
                slideIn: {
                    '0%': { transform: 'translateY(20px)', opacity: 0 },
                    '100%': { transform: 'translateY(0)', opacity: 1 },
                },
            },
            animation: {
                fadeIn: 'fadeIn 1s ease-in-out',
                slideIn: 'slideIn 0.8s ease-out',
            },
        },
    },
    plugins: [],
};
