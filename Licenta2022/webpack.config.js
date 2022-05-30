const webpack = require("webpack");

const path = require("path");

const { WebpackManifestPlugin } = require("webpack-manifest-plugin");

module.exports = {
  entry: "./Content/components/expose-components.js",
  output: {
    filename: "[name].js",
    globalObject: "this",
    path: path.resolve(__dirname, "dist/"),
    publicPath: "/dist/",
  },
  resolve: {
    extensions: [".ts", ".js"],
    fallback: {
      stream: require.resolve("stream-browserify"),
      buffer: require.resolve("buffer"),
    },
  },
  mode: process.env.NODE_ENV === "production" ? "production" : "development",
  optimization: {
    runtimeChunk: {
      name: "runtime", // necessary when using multiple entrypoints on the same page
    },
    splitChunks: {
      cacheGroups: {
        commons: {
          test: /[\\/]node_modules[\\/](react|react-dom)[\\/]/,
          name: "vendor",
          chunks: "all",
        },
      },
    },
  },
  module: {
    rules: [
      {
        test: /\.jsx?$/,
        exclude: /node_modules/,
        loader: "babel-loader",
      },
      {
        test: /\.tsx?$/,
        exclude: /node_modules/,
        loader: "ts-loader",
      },
      {
        test: /\.css$/i,
        use: ["style-loader", "css-loader"],
      },
    ],
  },
  plugins: [
    new webpack.ProvidePlugin({
      Buffer: ["buffer", "Buffer"],
    }),
    new webpack.ProvidePlugin({
      process: "process/browser",
    }),

    new WebpackManifestPlugin({
      fileName: "asset-manifest.json",
      generate: (seed, files) => {
        const manifestFiles = files.reduce((manifest, file) => {
          manifest[file.name] = file.path;
          return manifest;
        }, seed);
        const entrypointFiles = files
          .filter((x) => x.isInitial && !x.name.endsWith(".map"))
          .map((x) => x.path);
        return {
          files: manifestFiles,
          entrypoints: entrypointFiles,
        };
      },
    }),
  ],
};
