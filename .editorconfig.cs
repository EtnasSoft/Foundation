# .editorconfig
root = true

[*]
end_of_line = lf
insert_final_newline = true
charset = utf-8

[*.cs]
indent_style = space
indent_size = 4
csharp_new_line_before_open_brace = none
csharp_new_line_before_else = false
max_line_length = 80
trim_trailing_whitespace = true
insert_final_newline = true
dotnet_sort_system_directives_first = true
dotnet_separate_import_directive_groups = false
dotnet_diagnostic.IDE0005.severity = error # Using directive is unnecessary.
dotnet_diagnostic.IDE0130.severity = none # Namespace does not match file location.

# [*.{shader,hlsl,cginc,glsl}]
[*.shader]
csharp_new_line_before_open_brace = all
indent_style = space
indent_size = 4
max_line_length = 80
insert_final_newline = true
trim_trailing_whitespace = true

dotnet_diagnostic.IDE0005.severity = warning # Using directive is unnecessary.
