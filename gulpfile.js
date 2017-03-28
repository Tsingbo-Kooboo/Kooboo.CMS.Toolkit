/// <binding Clean='clean' />
"use strict";

var path = require("path"),
    gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

var webroot = "./Kooboo.CMS.Toolkit.Controls.RichTextEditors/Scripts/";

var paths = {
    js: webroot + "ueditor/**/*.js",
    minJs: webroot + "ueditor/**/*.min.js",
    css: webroot + "ueditor/**/*.css",
    minCss: webroot + "ueditor/**/*.min.css",
    staticFiles: webroot + "ueditor/**/*.{swf,htm,html,png,jpg,gif,min.js}",
    concatJsDest: "./Publish/Release/ueditor/ueditor.min.js",
    concatCssDest: "./Publish/Release/ueditor/ueditor.min.css",
    outputDir:"./Publish/Release/ueditor/"
};

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: webroot + 'ueditor/' })
        //.pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest(paths.outputDir));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        //.pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest(paths.outputDir));
});

gulp.task("min", ["min:js", "min:css"], function () {
    return gulp.src([paths.staticFiles])
    .pipe(gulp.dest(paths.outputDir));
});
