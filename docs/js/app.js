Vue.component('modal', {
    template: '#modal-template'
});

new Vue({
    el: '#app',
    data: {
        showModal: false
    },
    mounted: function() {
      $('body').toggleClass('loaded', true);
    }
});

$('.main').onepage_scroll({
  sectionContainer: 'section',
  easing: 'ease',
  animationTime: 500,
  pagination: true,
  updateURL: false,
  beforeMove: function(index) {

  },
  afterMove: function(index) {
    setTimeout(function() {
      $('section.active').toggleClass('inview', true);
    }, 250);
    var color = $('body').css('background-color');
    switch(index) {
      case 0:
        color = 'rgb(35,0,50);'
        break;
      case 1:
        color = 'rgb(40,0,55);'
        break;
      case 2:
        color = 'rgb(45,0,60);'
        break;
      case 3:
        color = 'rgb(50,0,65);'
        break;
    }
    $('body').css('transition', 'background-color ease 1s;');
    $('body').css('background-color', color);
  },
  loop: false,
  keyboard: true,
  direction: 'vertical'
});
