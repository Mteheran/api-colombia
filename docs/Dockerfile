# syntax=docker/dockerfile:1

FROM node:20-alpine AS build
WORKDIR /src

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run docs:build

FROM nginx:alpine AS runtime
COPY --from=build /src/.vitepress/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

