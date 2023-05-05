

////Wizard
//$('a[data-toggle="tab"]').on('show.bs.tab', function (e) {

  
//    var $target = $(e.target);

//    if ($target.hasClass('disabled')) {
//        return false;
//    }
//});

//$(".next-step").click(function (e) {
//    var $active = $('.wizard .nav-tabs .nav-item .active');
//    var $activeli = $active.parent("li");

//    $($activeli).next().find('a[data-toggle="tab"]').removeClass("disabled");
//    $($activeli).next().find('a[data-toggle="tab"]').click();
//});


//$(".prev-step").click(function (e) {

//    var $active = $('.wizard .nav-tabs .nav-item .active');
//    var $activeli = $active.parent("li");

//    $($activeli).prev().find('a[data-toggle="tab"]').removeClass("disabled");
//    $($activeli).prev().find('a[data-toggle="tab"]').click();

//});

//$(document).on('click', "ul.nav.nav-tabs li", function (e) {
//    var $this = $(this);
//    if ($this.find('a span.disa_step_new').length > 0) return;
//    $("ul.nav.nav-tabs li").find('a').each(function (index) {
//        if ($(this).hasClass("active")) {
//            $(this).removeClass("active");
//            $(this).addClass("disabled");
//        }
//    });
//    $this.find('a').each(function (index) {
//        if ($(this).hasClass("disabled")) {
//            $(this).removeClass("disabled");
//            $(this).addClass("active");
//        }
//    });
//});