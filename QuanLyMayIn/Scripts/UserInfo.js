

    var loadFile1 = function (event) {
        var image = document.getElementById('user-image1');
        image.src = URL.createObjectURL(event.target.files[0])
    }

    function FnSuccess_Password(data) {
        $('.closeform_1').click()
        Swal.fire({
        icon: 'success',
            title: 'Thành Công',
            text: `Đổi mật khẩu thành công`,
            timer: 2000,
            showConfirmButton: false,
        })
        location.reload();
    }



function FnSuccess_DoiEmail(data) {
   
    if (data == true) {
        $('.closeform_1').click()
        Swal.fire({
            icon: 'success',
            title: 'Thành Công',
            text: `Đổi email thành công`,
            timer: 2000,
            showConfirmButton: false,
        })
        location.reload()
    } else if (data == false) {
        $('.closeform_1').click()
        Swal.fire({
            icon: 'error',
            title: 'Lỗi',
            text: `Bạn đã nhập sai mật khẩu`,
            timer: 2000,
            showConfirmButton: false,
        })
    }
}
function FnBegin() {
    $('.loading_newtaikhoan').removeClass('d-none');
}



function FnFailure_Password(data) {
    $('.closeform_1').click()
    Swal.fire({
    icon: 'error',
        title: 'Lỗi',
        text: `Đổi mật khẩu không thành công`,
        timer: 2000,
        showConfirmButton: false,
    })
}

function FnFailure_DoiAnh(data) {
    $('.closeform_1').click()
    Swal.fire({
    icon: 'error',
        title: 'Lỗi',
        text: `Đổi ảnh đại diện không thành công`,
        timer: 2000,
        showConfirmButton: false,
    })
}

function FnFailure_DoiEmail(data) {
    $('.closeform_1').click()
    Swal.fire({
    icon: 'error',
        title: 'Lỗi',
        text: `Đổi email không thành công`,
        timer: 2000,
        showConfirmButton: false,
    })
}

$(document).ready(function () {

        jQuery.validator.addMethod("NotAllowNumber", function (value, element) {
            return this.optional(element) || /^([^0-9]*)$/.test(value);
        }, "Không được phép có chữ số.");

        jQuery.validator.addMethod("NotAllowFirstSpace", function (value, element) {
            return this.optional(element) || /^\S{1}/.test(value);
        }, "Kí tự đầu tiên không được có khoảng trắng.");

        jQuery.validator.addMethod("NotAllowSpecial", function (value, element) {
            return this.optional(element) || /^([^*\u0040.!/'#$%^&(){ }[:;<>,.?/~_+-=|]*)$/.test(value);
    }, "Không được phép có kí tự đặc biệt.");

        var form = $(".formDoiMatKhau").validate({
            onfocusout: function (element) {
            $(element).valid();
            },
            invalidHandler: function (form, validator) {
            validator.focusInvalid();
                Swal.fire({
            icon: 'error',
                    title: 'Xuất hiện lỗi',
                    text: `Đổi mật khẩu không thành công`,
                    timer: 2000,
                    showConfirmButton: false
                })

            },
            errorClass: "is-invalid",
            validClass: "is-valid",
            rules: {
            MatKhau1: {
                required: true,
                remote: {
                    url: "/DM_NhanVien/CheckMatKhau/",
                    type: "post",
                    dataType: "json",
                }
            },
            MatKhauMoi: {
                required: true,
                 minlength: 8
            },
            ReMatKhauMoi: {
                equalTo: '[name="MatKhauMoi"]'

                },
            },
            messages: {
            MatKhau1: {
            required: "Mật khẩu hiện tại không được bỏ trống",
                    remote: "Mật khẩu hiện tại không chính xác"
                },
                MatKhauMoi: {
            required: "Mật khẩu mới không được bỏ trống",
                    minlength: "Mật khẩu ít nhất 8 kí tự"
                },
                ReMatKhauMoi: {
            equalTo: "Mật khẩu mới không trùng khớp"
                },

            }
        });

        var form = $(".formDoiEmail").validate({
            onfocusout: function (element) {
                $(element).valid();
            },
            invalidHandler: function (form, validator) {
                validator.focusInvalid();
                Swal.fire({
                    icon: 'error',
                    title: 'Xuất hiện lỗi',
                    text: `Đổi email không thành công`,
                    timer: 2000,
                    showConfirmButton: false
                })

            },
            errorClass: "is-invalid",
            validClass: "is-valid",
            rules: {
                email1: {
                    required: true,

                },
                email2: {
                    required: true,
                    remote: {
                        url: "/DM_NhanVien/EmailTonTai_DoiEmail/",
                        type: "post",
                        dataType: 'json',

                    },
                    email: true
                },
                MatKhau1: {
                    required: true,
                    remote: {
                       url: "/DM_NhanVien/CheckMatKhau/",
                       type: "post",
                       }
                },
            },
            messages: {
                email1: {
                    required: "Email hiện tại không được bỏ trống",
                },
                email2: {
                    required: "Email mới không được bỏ trống",
                    remote: "Email đã được sử dụng",
                    email: "Email không hợp lệ"
                },
                MatKhau1: {
                    required: "Mật khẩu hiện tại không được bỏ trống",
                    remote: "Mật khẩu hiện tại không đúng"
                },
            }
        });

        $(".closeform_1").click(function () {

            $(':input', '.formDoiMatKhau')
                .not(':button, :submit, :reset, :hidden')
                .val('')
                .prop('checked', false)
                .prop('selected', false)
                .removeClass('is-invalid')
                .removeClass('is-valid')
          
        })
    })



$.post("/DM_NhanVien/GetNhanVienByIdCookei", (data) => {
    console.log(data)
    $("#emailCu").val(data.Email)
    $("#idCanBo").val(data.Ma_CanBo)
})