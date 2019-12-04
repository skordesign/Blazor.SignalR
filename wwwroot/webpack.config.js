const path = require("path");
const CleanWebpackPlugin = require("clean-webpack-plugin");

module.exports = { 
    entry: {
    "Blazor.signalr": "./src/InitializeSignalR.ts"
    },
    output: {
        path: path.join(__dirname, "/dist"),
        filename: "Blazor.signalr.js"
    },
    resolve: {
        extensions: [".js", ".ts"]
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: "ts-loader"
            }
        ]
    },
    plugins: [
        new CleanWebpackPlugin(["dist/*"])
    ]
};