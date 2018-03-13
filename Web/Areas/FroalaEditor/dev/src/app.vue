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
    }
  },
  data() {
    console.log(this.$attrs)
    return {
      config: null,
      model: this.value
    }
  },
  created() {
    let config = {
      events: {
        'froalaEditor.initialized': function() {
          console.log('initialized')
        },
        'froalaEditor.focus': function(e, editor) {
          console.log(editor.selection.get())
        }
      },
      toolbarButtons: [
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
      ],
      imageAllowedTypes: ['jpeg', 'jpg', 'png', 'gif'],
      imageUploadURL: '/Cms_Data/Contents/SampleSite',
      imageManagerLoadURL: `/Contents/Froala${location.search}`,
      heightMax: 300,
      heightMin: 100
    }
    for (const key in this.$attrs) {
      if (this.$attrs.hasOwnProperty(key)) {
        config[key] = this.$attrs[key]
      }
    }
    this.config = config
    console.log(this.config)
  }
}
</script>

<style>
.fr-buttons button,
.fr-box button {
  min-width: auto !important;
}
</style>

