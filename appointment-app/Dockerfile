FROM node:18-alpine AS builder
WORKDIR /usr/src/app
COPY ./appointment-app/package*.json ./
RUN npm install
COPY ./appointment-app/ ./
RUN npm run build

FROM nginx:alpine
WORKDIR /usr/share/nginx/html
RUN rm -rf ./*
COPY --from=builder /usr/src/app/build .