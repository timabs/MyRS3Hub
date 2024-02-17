/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./Components/**/*.razor"],
  theme: {
    extend: {
      fontFamily: {
        helv: ["Helvetica Neue", "sans-serif"],
        fantasy: ["MedievalSharp", "cursive"],
      },
      colors: {
        "regal-blue": "#2e4464",
      },
    },
  },
  plugins: [],
};
