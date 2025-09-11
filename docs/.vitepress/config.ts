import { defineConfig } from 'vitepress'
import { useSidebar } from 'vitepress-openapi'
import spec from '../public/openapi.json' with { type: 'json' }

const sidebar = useSidebar({
  spec,
})

export default defineConfig({
  lang: 'es-ES',
  title: 'API Colombia',
  description: 'Un proyecto Open-source que ofrece acceso a datos\n' +
    'sobre su diversidad, todo a tu alcance.',
  themeConfig: {
    nav: [
      { text: 'API Reference', link: '/' },
      { text: 'GitHub', link: 'https://github.com/Mteheran/api-colombia' }
    ],
    sidebar: [
      {
        text: 'Introduction',
        link: '/',
      },
      ...sidebar.generateSidebarGroups(),
    ],
    socialLinks: [
      { icon: 'github', link: 'https://github.com/Mteheran/api-colombia' }
    ],
    footer: {
      message: 'MIT License',
      copyright: 'Copyright © 2025 API Colombia'
    },
  },
  transformPageData(pageData) {
    const pageTitle = pageData.params?.pageTitle;

    if (pageTitle) {
      pageData.title = pageTitle;
      pageData.frontmatter ??= {};
      pageData.frontmatter.title = pageTitle;
    }
  },
})
