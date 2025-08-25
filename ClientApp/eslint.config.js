import antfu from '@antfu/eslint-config'

export default antfu({
  vue:        true,
  typescript: true,
  rules:      {
    'style/indent': [
      'error',
      2,
      {
        offsetTernaryExpressions: false,
        SwitchCase:               1,
      },
    ],
    'style/no-multi-spaces': [
      'error',
      {
        exceptions: {
          VariableDeclarator: true,
          TSTypeAnnotation:   true,
          PropertyDefinition: true,
        },
      },
    ],
    'style/array-bracket-spacing': [ 'error', 'always' ],
    'style/key-spacing':           [
      'error',
      {
        align: {
          afterColon: true,
          on:         'value',
        },
      },
    ],
    'ts/comma-dangle':       'off',
    'style/comma-dangle':    'off',
    'style/padded-blocks':   0,
    'no-prototype-builtins': 'off',
    'style/space-in-parens': [ 'error', 'always', { exceptions: [ '()', '{}' ] } ],
    'vue/block-tag-newline': [
      'error',
      {
        singleline:    'always',
        multiline:     'always',
        maxEmptyLines: 1,
      },
    ],
    'vue/multiline-html-element-content-newline': [
      'error',
      {
        ignoreWhenEmpty: true,
        allowEmptyLines: true,
      },
    ],
    'no-use-before-define':          'off',
    'ts/no-use-before-define':       'off',
    'unused-imports/no-unused-vars': 'off',
  },
})
