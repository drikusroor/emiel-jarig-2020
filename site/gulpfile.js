/*
Gulp is a task runner that handles task like
- compile Sass to CSS
- concatenation and minification of js, css files
- watch for changes in a directory
- run a local server and reload the browser during development
*/
var gulp = require('gulp');
var gulpIf = require('gulp-if');

var sass = require('gulp-sass'); /* converts to css */
var useref = require('gulp-useref'); /* concatenation of multiple files into one */
var uglify = require('gulp-uglify'); /* minify js content */
var cssnano = require('gulp-cssnano'); /* minify css content */
var del = require('del'); /* delete file/folder */
var runSequence = require('run-sequence'); /* run tasks in sequence */

var browserSync = require('browser-sync').create();

/*Variables*/
var scss_files_path = 'app/scss/**/*.scss';
var html_files_path = 'app/*.html';
var js_files_path = 'app/js/**/*.js';
var font_files_path = 'app/fonts/**/*';

var destination_folder = 'dist';

/* Task to convert scss (sass) to css */
gulp.task('sass2css', function () {
  return gulp.src(scss_files_path)
    .pipe(sass()) /* Converts Sass to CSS with gulp-sass */
    .pipe(gulp.dest('app/css'))
    .pipe(gulp.dest('dist/css'))
    .pipe(browserSync.reload({
      stream: true
    }))
});

/* Task to copy js */
gulp.task('js', function () {
  return gulp.src(js_files_path)
    .pipe(gulp.dest('dist/js'))
});

/* Task to concatenate js files mentioned in *.html into one file */
gulp.task('useref', function () {
  return gulp.src(html_files_path)
    .pipe(useref())
    /* Minifies only if it's a JavaScript file */
    .pipe(gulpIf('*.js', uglify()))
    /* Minifies only if it's a CSS file */
    .pipe(gulpIf('*.css', cssnano()))
    .pipe(gulp.dest(destination_folder))
});

/* Task to copy fonts/ folder to dist/ */
gulp.task('fonts', function () {
  return gulp.src(font_files_path)
    .pipe(gulp.dest(destination_folder + '/fonts'))
});

/* Task to remove dist/ folder */
gulp.task('clean:dist', function () {
  return del.sync(destination_folder);
});

/* Task to run local server for development and autoreload */
gulp.task('browserSync', function () {
  browserSync.init({
    server: {
      baseDir: 'app',
      https: true
    }
  })
});

/* Task to watch changes in specific folder and perform a task */
gulp.task('watch', ['browserSync', 'sass2css'], function () {
  gulp.watch(scss_files_path, ['sass2css']);
  gulp.watch(html_files_path, browserSync.reload);
  gulp.watch(js_files_path, browserSync.reload);
});

/* Task aggregation to run various above task sequentially and parallely, and prepare dist/ folder
  Used when deploying to staging/production */
gulp.task('build', function (callback) {
  console.log('Building files');

  runSequence('clean:dist',
    ['sass2css', 'js', 'useref', 'fonts'],
    callback
  )
});

/* Allows to use 'gulp' command to start development server */
gulp.task('default', function (callback) {
  runSequence(['sass2css', 'js', 'browserSync', 'watch'],
    callback
  )
})