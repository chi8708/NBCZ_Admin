var gulp = require("gulp");
var browserSync = require('browser-sync');
var concat = require("gulp-concat");

gulp.task('serve', function () {
    browserSync({
        file: "**",
        server: {
            baseDir: 'src'
        }
    });
    gulp.watch(["**/*.html", "**/*.css", "**/*.js"]).on('change', browserSync.reload);
});


gulp.task('concat', function () {
    var path = 'easyui/themes/super'
    gulp.src([path + '/*.css'])
        .pipe(concat('easyui.css'))
        .pipe(gulp.dest(path))
});

gulp.task('watch', function () {
});