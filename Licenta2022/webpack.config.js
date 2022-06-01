const webpack = require("webpack");

const path = require("path");

const { WebpackManifestPlugin } = require("webpack-manifest-plugin");

const MiniCssExtractPlugin = require("mini-css-extract-plugin");

const SpeedMeasurePlugin = require("speed-measure-webpack-plugin")

const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');

const smp = new SpeedMeasurePlugin()

module.exports = {
  context: __dirname,

  entry: "./Content/components/expose-components.js",
  output: {
    filename: "[name].js",
    globalObject: "this",
    path: path.resolve(__dirname, "dist/"),
    publicPath: "/dist/",
  },
  resolve: {
    extensions: [".ts", ".js", ".tsx"],
    fallback: {
      stream: require.resolve("stream-browserify"),
      buffer: require.resolve("buffer"),
      crypto: require.resolve("crypto-browserify")
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
        include: path.resolve(__dirname, 'Content/components'),
        loader: "babel-loader",
      },
      {
        test: /\.tsx?$/,
        exclude: /node_modules/,
        include: path.resolve(__dirname, 'Content/components'),
        loader: "babel-loader",
      },
      {
        test: /\.css$/i,
        use: [MiniCssExtractPlugin.loader, "css-loader"],
      },
    ],
  },
  plugins: [
    new MiniCssExtractPlugin(),

    new ForkTsCheckerWebpackPlugin(),

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
}