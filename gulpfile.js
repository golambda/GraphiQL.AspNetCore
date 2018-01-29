var gulp = require('gulp');
var concat = require('gulp-concat');
var webpack = require('webpack-stream');
var less = require('gulp-less');
var path = require('path');
var browserSync = require('browser-sync').create();
var exec = require('gulp-exec')

var config = {
    dest: "GraphiQL.AspNetCore/assets"
};

gulp.task('js', function () {
    return gulp.src('src/js/index.tsx')
        .pipe(webpack(require('./webpack.config.js')).on('error', function (e) {
            console.log(e);
        }))
        .pipe(gulp.dest(config.dest));
});

gulp.task('less', function () {
    return gulp.src(['src/css/**/*.less', 'src/css/**/*.css'])
        .pipe(less({
            paths: [path.join(__dirname, 'less', 'includes')]
        }))
        .pipe(concat('style.css'))
        .pipe(gulp.dest(config.dest));
});

gulp.task('graphiql-css', function () {
    return gulp.src('node_modules/graphiql/graphiql.css')
        .pipe(concat('graphiql.css'))
        .pipe(gulp.dest(config.dest));
});

gulp.task('copy-html', function () {
    gulp.src('index.html')
        .pipe(gulp.dest(config.dest));
});

gulp.task('build', [
    'js',
    'less',
    'graphiql-css',
    'copy-html']);

gulp.task('serve', ['build'], function () {
    browserSync.init({
        server: config.dest,
        ghostMode: false,
        port: 3010,
        ui: {
            port: 3011
        }
    });

    gulp.watch("src/css/**/*.less", ['less']);
    gulp.watch("src/js/**/*", ['js']);
    gulp.watch("index.html", ['copy-html']);
});
