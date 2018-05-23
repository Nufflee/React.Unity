import * as path from 'path'
import * as webpack from 'webpack'
import * as HtmlWebpackPlugin from 'html-webpack-plugin'
import { NewModule } from 'webpack'

export default function webpackConfig() {
  const config: webpack.Configuration = {
    entry: [
      path.join(__dirname, 'test.tsx')
    ],
    mode: 'development',
    output: {
      filename: '[name].bundle.js',
      path: path.join(__dirname, 'dist')
    },
    devtool: 'source-map',
    resolve: {
      extensions: ['.ts', '.tsx', '.js']
    },
    module: {
      rules: [
        {
          test: /\.(tsx|ts)?$/,
          enforce: 'pre',
          use: [
            {
              loader: 'tslint-loader',
              options: {
                fix: true
              }
            }
          ]
        },
        {
          test: /\.(tsx|ts)?$/,
          use: [
            {
              loader: 'ts-loader',
              query: {
                compilerOptions: {
                  sourceMap: true,
                  module: 'esnext'
                }
              }
            }
          ]
        },
        {test: /\.(png|jpe?g)$/, use: 'file-loader'}
      ]
    },
    plugins: [
      new HtmlWebpackPlugin({
        filename: 'index.html',
        title: 'React Unity Renderer'
      }) as any
    ]
  } as webpack.Configuration

  for (const rule of (config.module as NewModule).rules) {
    rule.exclude = /(node_modules|tests)/
  }

  return config
}