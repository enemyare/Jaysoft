/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      'inter': ['Inter', 'sans-serif'],
      colors:{
        'primary': '#F36E24',
        'primary-hover':'#F37E3D',
        'primary-text': '#1B1B1B',
        'secondary-text': '#818181',
        'danger': '#FF2F00',
        'background': '#F9F9F9',
        'secondary-bg': '#E6E6E6',
      }
    },
  },
  plugins: [],
}