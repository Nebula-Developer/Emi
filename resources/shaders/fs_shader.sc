$input v_normal

#include <bgfx_shader.sh>

void main()
{
    gl_FragColor = vec4(abs(v_normal), 1.0);
}
