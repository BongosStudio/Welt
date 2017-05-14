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
  },
  loop: false,
  keyboard: true,
  direction: 'vertical'
});
