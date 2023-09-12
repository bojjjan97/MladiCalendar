# Stage 1: Build an Angular Docker Image
FROM node:latest as build
WORKDIR /apps
COPY . .
RUN npm install
COPY . /apps
ARG configuration=production
RUN npm run build --outputPath=./dist/out
# Stage 2, use the compiled app, ready for production with Nginx
FROM nginx:alpine
COPY /nginx-custom.conf /etc/nginx/conf.d/
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /apps/dist/web-app/ /usr/share/nginx/html
