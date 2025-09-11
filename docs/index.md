---
aside: false
outline: false
title: vitepress-openapi
---

<script setup>
import { useOpenapi } from 'vitepress-openapi/client'

const spec = useOpenapi().getSpec()
const tags = useOpenapi().getTags()

const operations = Object.entries(spec.paths).flatMap(([path, methods]) => {
  return Object.entries(methods).map(([method, operation]) => ({
    path,
    method,
    operationId: operation.operationId,
    summary: operation.summary || operation.operationId,
    tags: operation.tags || [],
  }))
}).filter(Boolean)

const operationsGroupedByTags = tags.reduce((acc, tag) => {
  acc[tag.name] = operations.filter(op => op.tags.includes(tag.name));
  return acc;
}, {});
</script>

<OAInfo />

## Base URL

The base URL for the API is:

```text
https://api-colombia.com
```

## API Endpoints

<div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(300px, 1fr)); gap: 20px;">
    <div v-for="tag in tags" :key="tag.name">
        <h3>{{ tag.name }}</h3>
        <div style="display: flex; flex-direction: column; gap: 10px; margin-top: 10px;">
            <OAOperationLink v-for="operation in operationsGroupedByTags[tag.name]" :key="operation.operationId" :href="`/operations/${operation.operationId}`" :method="operation.method" :title="operation.summary">
            </OAOperationLink>
        </div>
    </div>
</div>
