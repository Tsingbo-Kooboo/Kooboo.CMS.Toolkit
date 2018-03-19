import Vue from 'vue'
import App from './app.vue'
// Require Froala Editor js file.
// require('froala-editor/js/froala_editor.pkgd.min')

// Require Froala Editor css files.
// require('froala-editor/css/froala_editor.pkgd.min.css')
// require('font-awesome/css/font-awesome.css')
// require('froala-editor/css/froala_style.min.css')
import VueFroala from 'vue-froala-wysiwyg'
Vue.use(VueFroala)
const wrapperSelector = '.kb-froala-editor'

document.querySelectorAll(wrapperSelector).forEach((el, index) => {
  let app = new Vue({
    components: {
      'kb-froala-editor': App
    }
  })
  app.$mount(el)
})
