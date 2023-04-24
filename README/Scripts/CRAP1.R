if (!require(ggplot2)) {
  install.packages("ggplot2")
  library(ggplot2)
}

# function to calculate CRAP1 score
calculate_crap1 <- function(comp, cov) {
  (comp^2) * ((1 - (cov/100))^3) + comp
}

comp_value <- 30
cov_values <- seq(0, 100, by = 5)

# calculate CRAP1 scores
crap1_scores <- sapply(cov_values, function(cov) {
  calculate_crap1(comp_value, cov)
})


df <- data.frame(cov = cov_values, crap1 = crap1_scores)

p <- ggplot(df, aes(x = cov, y = crap1)) +
  geom_ribbon(aes(ymin = 0, ymax = crap1), fill = "red", alpha = 0.2) +
  labs(x = "Code Coverage (%)", y = "CRAP1 Score", title = paste0("CRAP1 Score for Complexity = ", comp_value)) +
  theme(plot.title = element_text(hjust = 0.5, size = 16))

ggsave("CRAP1 score.png", p, width = 20, height = 10, dpi = 300)