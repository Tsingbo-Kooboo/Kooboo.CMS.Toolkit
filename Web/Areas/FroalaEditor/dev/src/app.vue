<template>
  <div id="app">
    <froala tag="textarea"
            :config="config"
            v-model="model"></froala>
  </div>
</template>

<script>
import VueFroala from 'vue-froala-wysiwyg'

export default {
  name: 'app',
  props: {
    value: {
      type: String
    },
    name: {
      type: String
    },
    toolbarButtons: {
      type: Array,
      default() {
        return [
          'fullscreen',
          '|',
          'insertLink',
          'insertImage',
          'insertVideo',
          'insertFile',
          'insertTable',
          '|',
          'quote',
          'insertHR',
          'subscript',
          'superscript',
          'undo',
          'redo',
          '-',
          'bold',
          'italic',
          'underline',
          'strikeThrough',
          '|',
          'fontFamily',
          '|',
          'fontSize',
          '|',
          'color',
          'emoticons',
          'inlineStyle',
          '-',
          'paragraphFormat',
          '|',
          'paragraphStyle',
          'align',
          'formatOL',
          'formatUL',
          'outdent',
          'indent',
          'clearFormatting'
        ]
      }
    }
  },
  data() {
    console.log(this.toolbarButtons)
    return {
      config: null,
      model: this.value
    }
  },
  created() {
    fetch(
      `/Contents/Froala/Options${location.search}&columnName=${this.name}`,
      {
        credentials: 'include'
      }
    )
      .then(response => response.json())
      .then(options => {
        console.log(['cccc', options])
      })
    let config = {
      events: {
        'froalaEditor.initialized': function() {
          console.log('initialized')
        },
        'froalaEditor.focus': function(e, editor) {
          console.log(editor.selection.get())
        }
      },
      imageAllowedTypes: ['jpeg', 'jpg', 'png', 'gif'],
      imageUploadURL: `/Contents/Froala/Upload${location.search}`,
      imageManagerLoadURL: `/Contents/Froala${location.search}`,
      heightMax: 300,
      heightMin: 100
    }
    // for (const key in this.$attrs) {
    //   if (this.$attrs.hasOwnProperty(key)) {
    //     config[key] = this.$attrs[key]
    //   }
    // }
    this.config = Object.assign({}, config, {
      toolbarButtons: this.toolbarButtons
    })
    console.log(this.config)
  },
  mounted() {
    this.$nextTick(() => {
      $('.fr-wrapper [href="https://www.froala.com/wysiwyg-editor?k=u"]')
        .parent()
        .remove()
    })
  }
}
</script>

<style>
.fr-buttons button,
.fr-box button {
  min-width: auto !important;
}
</style>

