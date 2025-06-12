# This file is auto-generated from the current state of the database. Instead
# of editing this file, please use the migrations feature of Active Record to
# incrementally modify your database, and then regenerate this schema definition.

ActiveRecord::Schema[7.0].define(version: 2025_06_03_141825) do
  create_table "users", force: :cascade do |t|
    t.string "email", null: false
    t.string "password_hash", null: false
    t.string "first_name"
    t.string "last_name"
    t.boolean "is_active", default: true
    t.boolean "is_deleted", default: false
    t.integer "role_id", null: false
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["email"], name: "index_users_on_email", unique: true
    t.index ["is_active"], name: "index_users_on_is_active"
    t.index ["role_id"], name: "index_users_on_role_id"
  end

  create_table "user_profiles", force: :cascade do |t|
    t.integer "user_id", null: false
    t.string "display_name"
    t.text "bio"
    t.string "avatar_url"
    t.boolean "is_profile_public", default: true
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["user_id"], name: "index_user_profiles_on_user_id", unique: true
    t.index ["is_profile_public"], name: "index_user_profiles_on_is_profile_public"
  end

  create_table "roles", force: :cascade do |t|
    t.string "role_name"
    t.boolean "is_deleted", default: false
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["role_id"], name: "index_roles_on_role_id"
  end

  create_table "categories", force: :cascade do |t|
    t.string "name", null: false
    t.string "slug", null: false
    t.text "description"
    t.integer "user_id"
    t.boolean "is_deleted", default: false
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["name"], name: "index_categories_on_name", unique: true
    t.index ["slug"], name: "index_categories_on_slug"
    t.index ["user_id"], name: "index_categories_on_user_id"
    t.index ["created_at"], name: "index_categories_on_created_at"
  end

  create_table "posts", force: :cascade do |t|
    t.string "title", null: false
    t.text "content", null: false
    t.string "slug", null: false
    t.integer "user_id", null: false
    t.integer "category_id", null: false
    t.boolean "is_scam", default: false
    t.boolean "is_deleted", default: false
    t.string "status"
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["slug"], name: "index_posts_on_slug"
    t.index ["user_id"], name: "index_posts_on_user_id"
    t.index ["category_id"], name: "index_posts_on_category_id"
    t.index ["is_scam"], name: "index_posts_on_is_scam"
    t.index ["created_at"], name: "index_posts_on_created_at"
  end

  create_table "comments", force: :cascade do |t|
    t.text "content", null: false
    t.integer "user_id", null: false
    t.integer "post_id", null: false
    t.integer "reply_to"
    t.boolean "is_deleted", default: false
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["post_id"], name: "index_comments_on_post_id"
    t.index ["user_id"], name: "index_comments_on_user_id"
    t.index ["created_at"], name: "index_comments_on_created_at"
    t.index ["reply_to"], name: "index_comments_on_reply_to"
  end

  create_table "scam_detections", force: :cascade do |t|
    t.integer "post_id", null: false
    t.float "model_prediction"
    t.text "detection_details"
    t.boolean "is_deleted", default: false
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["post_id"], name: "index_scam_detections_on_post_id", unique: true
    t.index ["model_prediction"], name: "index_scam_detections_on_model_prediction"
    t.index ["created_at"], name: "index_scam_detections_on_created_at"
  end

  create_table "reports", force: :cascade do |t|
    t.integer "user_id", null: false
    t.integer "post_id"
    t.integer "comment_id"
    t.string "reason", null: false
    t.text "details"
    t.boolean "is_deleted", default: false
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["user_id"], name: "index_reports_on_user_id"
    t.index ["post_id"], name: "index_reports_on_post_id"
    t.index ["comment_id"], name: "index_reports_on_comment_id"
    t.index ["created_at"], name: "index_reports_on_created_at"
  end

  create_table "report_statuses", force: :cascade do |t|
    t.integer "report_id", null: false
    t.string "status", null: false
    t.integer "handled_by"
    t.boolean "is_processed", default: false
    t.boolean "is_deleted", default: false
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["report_id"], name: "index_report_statuses_on_report_id", unique: true
    t.index ["is_processed"], name: "index_report_statuses_on_is_processed"
    t.index ["handled_by"], name: "index_report_statuses_on_handled_by"
  end

  create_table "notifications", force: :cascade do |t|
    t.integer "user_id", null: false
    t.integer "sender_id"
    t.string "type", null: false
    t.text "message", null: false
    t.boolean "is_read", default: false
    t.boolean "is_deleted", default: false
    t.integer "post_id"
    t.integer "comment_id"
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["user_id"], name: "index_notifications_on_user_id"
    t.index ["sender_id"], name: "index_notifications_on_sender_id"
    t.index ["is_read"], name: "index_notifications_on_is_read"
    t.index ["created_at"], name: "index_notifications_on_created_at"
    t.index ["post_id"], name: "index_notifications_on_post_id"
    t.index ["comment_id"], name: "index_notifications_on_comment_id"
  end

  create_table "likes", force: :cascade do |t|
    t.integer "user_id", null: false
    t.integer "post_id"
    t.integer "comment_id"
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["user_id"], name: "index_likes_on_user_id"
    t.index ["created_at"], name: "index_likes_on_created_at"
    t.index ["user_id", "post_id"], name: "index_likes_on_user_id_and_post_id", unique: true, where: "post_id IS NOT NULL"
    t.index ["user_id", "comment_id"], name: "index_likes_on_user_id_and_comment_id", unique: true, where: "comment_id IS NOT NULL"
  end

  create_table "bookmarks", force: :cascade do |t|
    t.integer "user_id", null: false
    t.integer "post_id", null: false
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["user_id"], name: "index_bookmarks_on_user_id"
    t.index ["post_id"], name: "index_bookmarks_on_post_id"
    t.index ["created_at"], name: "index_bookmarks_on_created_at"
    t.index ["user_id", "post_id"], name: "index_bookmarks_on_user_id_and_post_id", unique: true
  end

  create_table "activity_logs", force: :cascade do |t|
    t.integer "user_id", null: false
    t.string "action", null: false
    t.text "details"
    t.string "table_name"
    t.integer "record_id"
    t.boolean "is_deleted", default: false
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["user_id"], name: "index_activity_logs_on_user_id"
    t.index ["created_at"], name: "index_activity_logs_on_created_at"
    t.index ["table_name"], name: "index_activity_logs_on_table_name"
    t.index ["record_id"], name: "index_activity_logs_on_record_id"
  end

  create_table "warnings", force: :cascade do |t|
    t.integer "user_id", null: false
    t.string "reason", null: false
    t.text "description", null: false
    t.integer "issued_by", null: false
    t.boolean "is_deleted", default: false
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["user_id"], name: "index_warnings_on_user_id"
    t.index ["issued_by"], name: "index_warnings_on_issued_by"
    t.index ["created_at"], name: "index_warnings_on_created_at"
  end

  create_table "attachments", force: :cascade do |t|
    t.string "file_url", null: false
    t.string "file_type", null: false
    t.string "file_size", null: false
    t.integer "post_id"
    t.integer "comment_id"
    t.integer "user_id", null: false
    t.boolean "is_deleted", default: false
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["post_id"], name: "index_attachments_on_post_id"
    t.index ["comment_id"], name: "index_attachments_on_comment_id"
    t.index ["user_id"], name: "index_attachments_on_user_id"
    t.index ["file_type"], name: "index_attachments_on_file_type"
    t.index ["created_at"], name: "index_attachments_on_created_at"
  end

  create_table "system_configs", force: :cascade do |t|
    t.string "key", null: false
    t.text "value"
    t.string "description"
    t.datetime "created_at", null: false
    t.datetime "updated_at"
    t.index ["key"], name: "index_system_configs_on_key", unique: true
  end

  # Foreign Key Constraints
  add_foreign_key "users", "roles"
  add_foreign_key "user_profiles", "users"
  add_foreign_key "categories", "users"
  add_foreign_key "posts", "users"
  add_foreign_key "posts", "categories"
  add_foreign_key "comments", "users"
  add_foreign_key "comments", "posts"
  add_foreign_key "comments", "comments", column: "reply_to"
  add_foreign_key "scam_detections", "posts"
  add_foreign_key "reports", "users"
  add_foreign_key "reports", "posts"
  add_foreign_key "reports", "comments"
  add_foreign_key "report_statuses", "reports"
  add_foreign_key "report_statuses", "users", column: "handled_by"
  add_foreign_key "notifications", "users"
  add_foreign_key "notifications", "users", column: "sender_id"
  add_foreign_key "notifications", "posts"
  add_foreign_key "notifications", "comments"
  add_foreign_key "likes", "users"
  add_foreign_key "likes", "posts"
  add_foreign_key "likes", "comments"
  add_foreign_key "bookmarks", "users"
  add_foreign_key "bookmarks", "posts"
  add_foreign_key "activity_logs", "users"
  add_foreign_key "warnings", "users"
  add_foreign_key "warnings", "users", column: "issued_by"
  add_foreign_key "attachments", "posts"
  add_foreign_key "attachments", "comments"
  add_foreign_key "attachments", "users"
end 