const path = require("path");

console.log("Mode: ", process.env.NODE_ENV);
const publicPath = process.env.PUBLIC_PATH;
console.log("Public path: ", publicPath);

module.exports = {
    outputDir: path.resolve(__dirname, "./../wwwroot/dist"),
    publicPath,
    "transpileDependencies": [
      "vuetify"
    ]
};
