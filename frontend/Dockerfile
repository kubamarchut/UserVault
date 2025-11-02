FROM node:22.18 AS build

WORKDIR /app

COPY . .

RUN npm install

RUN npm run build

FROM nginx:alpine

COPY --from=build /app/dist/spa /usr/share/nginx/html

COPY default.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]